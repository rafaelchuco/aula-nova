"""Pruebas HTTP reales contra el laboratorio YA INICIADO en IIS Express.
Uso: python tools/probar_crud.py http://localhost:51109
Utiliza sesiones de prueba independientes; no modifica la sesión de tu navegador.
"""
import html
import http.cookiejar
import re
import sys
import urllib.error
import urllib.parse
import urllib.request

base = (sys.argv[1] if len(sys.argv) > 1 else 'http://localhost:51109').rstrip('/')

def client():
    return urllib.request.build_opener(urllib.request.HTTPCookieProcessor(http.cookiejar.CookieJar()))

a = client()
b = client()

def request(path, data=None, browser=a):
    encoded = None if data is None else urllib.parse.urlencode(data).encode()
    try:
        with browser.open(base + path, data=encoded, timeout=15) as response:
            return response.status, html.unescape(response.read().decode('utf-8'))
    except urllib.error.HTTPError as error:
        return error.code, html.unescape(error.read().decode('utf-8'))

def post(path, data):
    status, page = request(path)
    assert status == 200, (path, status)
    token = re.search(r'name="__RequestVerificationToken"[^>]*value="([^"]+)"', page)
    assert token, 'No se encontró el token antifalsificación.'
    return request(path, dict(data, __RequestVerificationToken=token[1]))

def ids(page):
    return sorted(set(int(i) for i in re.findall(r'/Alumno/Details/(\d+)', page)))

status, listing = request('/')
assert status == 200, 'Inicia el proyecto en Visual Studio antes de ejecutar esta prueba.'
assert len(ids(listing)) >= 3, 'El primer ingreso debe contener al menos tres ejemplos.'
student = dict(Nombres='PruebaHTTP', Apellidos='Laboratorio', Edad='22', Correo='prueba.http@example.com')
assert post('/Alumno/Create', student)[0] == 200
status, listing = request('/')
assert 'PruebaHTTP' in listing
identity = max(ids(listing))
assert 'PruebaHTTP' not in request('/', browser=b)[1], 'Las sesiones deben estar aisladas.'
status, details = request('/Alumno/Details/' + str(identity))
assert status == 200 and student['Correo'] in details
invalid = dict(student, Edad='0')
assert 'validation-summary-errors' in post('/Alumno/Create', invalid)[1]
edited = dict(student, Nombres='ActualizadoHTTP', Edad='24')
assert post('/Alumno/Edit/' + str(identity), edited)[0] == 200
assert 'ActualizadoHTTP' in request('/')[1], 'Actualizar el navegador debe conservar los cambios.'
assert 'ActualizadoHTTP' in request('/?buscar=ActualizadoHTTP')[1]
assert request('/Alumno/Details/9999999')[0] == 404
assert request('/Alumno/Delete/' + str(identity))[0] == 200
assert request('/Alumno/Details/' + str(identity))[0] == 200, 'GET Delete solo debe confirmar.'
assert post('/Alumno/Delete/' + str(identity), {})[0] == 200
assert 'ActualizadoHTTP' not in request('/')[1]
# Vaciar únicamente esta sesión de prueba; no debe reinicializarse al refrescar.
for identity in ids(request('/')[1]):
    assert post('/Alumno/Delete/' + str(identity), {})[0] == 200
assert ids(request('/')[1]) == [], 'La lista vacía no debe volver a cargar los ejemplos.'
assert len(ids(request('/', browser=b)[1])) >= 3
assert post('/Alumno/Create', student)[0] == 200, 'Debe ser posible crear desde una lista vacía.'
print('OK: ejemplos, crear, detalles, editar, eliminar con confirmación, refrescar, validación, búsqueda, sesión aislada y lista vacía persistente.')
