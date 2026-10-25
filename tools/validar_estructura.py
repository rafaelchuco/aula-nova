"""Auditoría documental del laboratorio. No sustituye ejecutar IIS Express."""
from pathlib import Path
import re
import xml.etree.ElementTree as ET

root = Path(__file__).resolve().parents[1]
project = root / 'LaboratorioCRUDAlumnos'
model = (project / 'Models/AlumnoModel.cs').read_text(encoding='utf-8')
controller = (project / 'Controllers/AlumnoController.cs').read_text(encoding='utf-8')
fields = re.findall(r'public\s+(int|string)\s+(\w+)\s*\{\s*get;\s*set;\s*\}', model)
assert fields == [('int', 'Id'), ('string', 'Nombres'), ('string', 'Apellidos'), ('int', 'Edad'), ('string', 'Correo')], fields
assert 'if (Session["alumnos"] == null)' in controller
assert 'return (List<AlumnoModel>)Session["alumnos"]' in controller
assert controller.count('new AlumnoModel { Id =') >= 3
assert controller.count('Session["alumnos"] = alumnos;') == 3
assert controller.count('[ValidateAntiForgeryToken]') == 3
assert not re.search(r'static\s+.*List<AlumnoModel>', controller)
assert 'Session["alumnos"].Count' not in controller
for view in ['Index', 'Details', 'Create', 'Edit', 'Delete']:
    assert (project / 'Views/Alumno' / (view + '.cshtml')).is_file()
for view in ['Create', 'Edit', 'Delete']:
    text = (project / 'Views/Alumno' / (view + '.cshtml')).read_text(encoding='utf-8')
    assert 'FormMethod.Post' in text
    assert '@Html.AntiForgeryToken()' in text
config = ET.parse(project / 'Web.config').getroot()
assert config.find('./system.web/sessionState').attrib['mode'] == 'InProc'
assert config.find('./system.web/authentication').attrib['mode'] == 'None'
assert config.find('./connectionStrings') is None
csproj = ET.parse(project / 'LaboratorioCRUDAlumnos.csproj')
ns = {'m': 'http://schemas.microsoft.com/developer/msbuild/2003'}
assert csproj.find('.//m:TargetFrameworkVersion', ns).text == 'v4.7.2'
assert csproj.getroot().get('Sdk') is None
for item in csproj.findall('.//m:Compile', ns) + csproj.findall('.//m:Content', ns):
    assert (project / item.attrib['Include'].replace('\\', '/')).is_file(), item.attrib
application_files = list(project.rglob('*.cs')) + list(project.rglob('*.config'))
for path in application_files:
    if 'obj' in path.parts or 'bin' in path.parts:
        continue
    text = path.read_text(encoding='utf-8')
    assert not re.search(r'Microsoft\.AspNetCore|Microsoft\.Data\.Sqlite|EntityFramework|DbContext|SqlConnection|File\.(Write|Read)', text), path
print('OK: MVC clásico .NET Framework 4.7.2; cinco campos exactos; sesiones InProc; tres ejemplos; GET/POST, CSRF y cinco vistas presentes; sin base de datos ni almacenamiento en archivos.')
print('Esta revisión comprueba código y configuración. Las pruebas funcionales y capturas requieren Windows + IIS Express.')
