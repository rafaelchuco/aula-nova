"""HTTP integration checks against a real ASP.NET Core process and temporary SQLite DB.
Run after dotnet build -c Release. Requires Python 3.10+; no extra packages.
"""
import contextlib
import html
import http.cookiejar
import os
from pathlib import Path
import re
import socket
import sqlite3
import subprocess
import tempfile
import time
import urllib.error
import urllib.parse
import urllib.request

ROOT = Path(__file__).resolve().parents[1]
DLL = ROOT / 'demo10/bin/Release/net10.0/AulaNova.dll'
assert DLL.exists(), 'Run dotnet build demo10/demo10.csproj -c Release first'

with tempfile.TemporaryDirectory(prefix='aulanova-test-') as temp:
    db = Path(temp) / 'test.db'
    with socket.socket() as sock:
        sock.bind(('127.0.0.1', 0))
        port = sock.getsockname()[1]
    base = f'http://127.0.0.1:{port}'
    env = dict(os.environ, ASPNETCORE_ENVIRONMENT='Development', Database__Path=str(db))
    log = open(Path(temp) / 'server.log', 'w+')
    process = None
    def start():
        global process
        process = subprocess.Popen(['dotnet', str(DLL), '--urls', base], cwd=ROOT / 'demo10', env=env, stdout=log, stderr=log)
        for _ in range(120):
            try:
                urllib.request.urlopen(base, timeout=1).close()
                return
            except (urllib.error.URLError, TimeoutError):
                if process.poll() is not None:
                    log.seek(0)
                    raise AssertionError(log.read())
                time.sleep(.25)
        raise AssertionError('Server startup timed out')
    def stop():
        if process is not None and process.poll() is None:
            process.terminate()
            try:
                process.wait(timeout=10)
            except subprocess.TimeoutExpired:
                process.kill()
                process.wait()
    client = urllib.request.build_opener(urllib.request.HTTPCookieProcessor(http.cookiejar.CookieJar()))
    def request(path, data=None):
        encoded = None if data is None else urllib.parse.urlencode(data).encode()
        try:
            with client.open(base + path, data=encoded, timeout=10) as response:
                return response.status, response.read().decode()
        except urllib.error.HTTPError as error:
            return error.code, error.read().decode()
    def post(path, data):
        status, page = request(path)
        assert status == 200, (path, status)
        token = re.search(r'name="__RequestVerificationToken"[^>]*value="([^"]+)"', page)
        assert token, 'Missing CSRF token'
        return request(path, dict(data, __RequestVerificationToken=html.unescape(token[1])))
    def contains(page, text):
        assert text in html.unescape(page), text
    student = dict(Nombres='Prueba', Apellidos='Integración', Documento='91234567', Correo='prueba@example.com', Telefono='999123456', Carrera='Ingeniería de Sistemas', Ciclo='4', Estado='Activo', Observaciones='<script>alert(1)</script>')
    try:
        start()
        status, page = request('/')
        assert status == 200
        contains(page, 'Tu comunidad empieza aquí')
        assert request('/css/site.css')[0] == 200
        assert request('/Alumno/Create', student)[0] == 400, 'CSRF must reject unprotected mutation'
        status, page = post('/Alumno/Create', student)
        assert status == 200
        contains(page, 'Prueba Integración')
        with sqlite3.connect(db) as connection:
            row = connection.execute('SELECT id,documento,ciclo FROM alumnos').fetchone()
            assert row[1:] == ('91234567', 4)
            identity = row[0]
        status, page = request(f'/Alumno/Details/{identity}')
        assert status == 200
        assert '<script>alert(1)</script>' not in page, 'Stored text must be HTML encoded'
        contains(page, student['Observaciones'])
        status, page = post('/Alumno/Create', dict(student, Correo='other@example.com'))
        contains(page, 'Ya existe un alumno')
        status, page = post('/Alumno/Create', dict(student, Documento='92345678', Correo='PRUEBA@EXAMPLE.COM'))
        contains(page, 'Ya existe un alumno')
        status, page = post('/Alumno/Create', dict(student, Documento='92345678', Correo='other@example.com', Ciclo='99'))
        assert 'validation-summary-errors' in page
        status, page = post('/Alumno/Create', dict(student, Documento='92345678', Correo='other@example.com', Carrera='Inventada'))
        contains(page, 'Selecciona una carrera')
        status, page = post('/Alumno/Create', dict(student, Nombres='   ', Documento='92345678', Correo='other@example.com'))
        assert 'validation-summary-errors' in page
        status, page = request('/?buscar=91234567')
        contains(page, 'Prueba Integración')
        status, page = request('/?estado=Egresado')
        contains(page, 'No encontramos coincidencias')
        assert request('/Alumno/Details/999999')[0] == 404
        edited = dict(student, Nombres='Actualizado', Estado='Egresado', Ciclo='10')
        assert post(f'/Alumno/Edit/{identity}', edited)[0] == 200
        contains(request('/?estado=Egresado')[1], 'Actualizado Integración')
        stop()
        start()
        contains(request('/')[1], 'Actualizado Integración')
        # The GET confirmation must preserve the row.
        assert request(f'/Alumno/Delete/{identity}')[0] == 200
        assert request(f'/Alumno/Details/{identity}')[0] == 200
        assert post(f'/Alumno/Delete/{identity}', {})[0] == 200
        contains(request('/')[1], 'Tu comunidad empieza aquí')
        with sqlite3.connect(db) as connection:
            assert connection.execute('SELECT count(*) FROM alumnos').fetchone()[0] == 0
            assert connection.execute('PRAGMA integrity_check').fetchone()[0] == 'ok'
        print('PASS: CRUD, SQLite persistence across restart, uniqueness, validation, filters, CSRF, HTML encoding, 404 and database integrity.')
    finally:
        stop()
        log.close()
