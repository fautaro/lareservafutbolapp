-- Crear un usuario due�o de ejemplo (necesario para los complejos)
INSERT INTO Usuario (Auth0Id, Nombre, Email, TipoUsuarioId)
SELECT 'dueno-ejemplo-1', 'Due�o Ejemplo', 'dueno@ejemplo.com',
       (SELECT Id FROM TipoUsuario WHERE Nombre = 'Dueno' LIMIT 1)
WHERE NOT EXISTS (SELECT 1 FROM Usuario WHERE Auth0Id = 'dueno-ejemplo-1');

-- Asegurar que existan al menos 2 ciudades para los ejemplos
INSERT INTO Ciudad (Nombre)
SELECT 'Viedma' WHERE NOT EXISTS (SELECT 1 FROM Ciudad WHERE Nombre = 'Viedma');

INSERT INTO Ciudad (Nombre)
SELECT 'Patagones' WHERE NOT EXISTS (SELECT 1 FROM Ciudad WHERE Nombre = 'Patagones');

-- Insertar deportes
INSERT INTO Deporte (Nombre, Icon, BgClass, TextClass) VALUES
('F�tbol', 'fas fa-futbol', 'bg-blue-600', 'text-white'),
('P�del', 'fas fa-table-tennis', 'bg-gray-200', 'text-black');

-- Insertar complejos
INSERT INTO Complejo (Nombre, CiudadId, DuenoId, Precio, Imagen, Categoria, DeporteId, DeportePillBg, DeportePillText) VALUES
-- Complejos de F�tbol en ciudad 1 (Ca�uelas)
('La tranquera Complejo Deportivo', 1, (SELECT Id FROM Usuario WHERE Auth0Id = 'dueno-ejemplo-1'), 45000.00, 'https://www.infocanuelas.com/media/luz-verde-para-el-funcionamiento-de-gimnasios-y-canchas-de-futbol-5-21942.jpg', 'F�tbol 5', 1, 'bg-blue-100', 'text-blue-800'),
('Tercer Tiempo', 1, (SELECT Id FROM Usuario WHERE Auth0Id = 'dueno-ejemplo-1'), 40000.00, 'https://i.ibb.co/FbyqzzWF/497928378-2885642464969002-7636997158516311138-n.jpg', 'F�tbol 5', 1, 'bg-blue-100', 'text-blue-800'),
('La Scaloneta Futbol 5', 1, (SELECT Id FROM Usuario WHERE Auth0Id = 'dueno-ejemplo-1'), 45000.00, 'https://distritofutbol.com.ar/media-library/constitucion-9-n2dbvs6y-double-picture.jpg', 'F�tbol 5', 1, 'bg-blue-100', 'text-blue-800'),
('De Rabona', 1, (SELECT Id FROM Usuario WHERE Auth0Id = 'dueno-ejemplo-1'), 35000.00, 'https://www.generallavalle.gob.ar/fotos/noticias/cancha-futbol-deportes-85.jpg', 'F�tbol 5', 1, 'bg-blue-100', 'text-blue-800'),
('Sol de Mayo', 1, (SELECT Id FROM Usuario WHERE Auth0Id = 'dueno-ejemplo-1'), 50000.00, 'https://i.ibb.co/nN5FYsk2/c3be3e45-c03b-4996-9aec-202e9ccc0a4c.png', 'F�tbol 5', 1, 'bg-blue-100', 'text-blue-800'),
('A un toque - Futbol 5', 1, (SELECT Id FROM Usuario WHERE Auth0Id = 'dueno-ejemplo-1'), 40000.00, 'https://www.hoysejuega.com/uploads/Modules/ImagenesComplejos/800_600_grun-3.jpg', 'F�tbol 5', 1, 'bg-blue-100', 'text-blue-800'),

-- Complejos de F�tbol en ciudad 2 (Buenos Aires)
('La vieja Bodega - Futbol 5', 2, (SELECT Id FROM Usuario WHERE Auth0Id = 'dueno-ejemplo-1'), 48000.00, 'https://i.ibb.co/6cHpD7GK/Chat-GPT-Image-31-may-2025-11-25-48.png', 'F�tbol 5', 1, 'bg-blue-100', 'text-blue-800'),
('Barsa Futbol 5', 2, (SELECT Id FROM Usuario WHERE Auth0Id = 'dueno-ejemplo-1'), 48000.00, 'https://www.diarioel9dejulio.com.ar/wp-content/uploads/2012/04/cancha28.jpg', 'F�tbol 5', 1, 'bg-blue-100', 'text-blue-800'),
('Barrilete C�smico', 2, (SELECT Id FROM Usuario WHERE Auth0Id = 'dueno-ejemplo-1'), 40000.00, 'https://i.ibb.co/N62JrpK5/barrilete.png', 'F�tbol 5', 1, 'bg-blue-100', 'text-blue-800'),

-- Complejos de P�del en ciudad 1 (Ca�uelas)
('Los Aromos Padel', 1, (SELECT Id FROM Usuario WHERE Auth0Id = 'dueno-ejemplo-1'), 25000.00, 'https://i.ibb.co/xSGvpmCC/los-aromos.png', 'P�del', 2, 'bg-green-100', 'text-green-800'),
('Tercer Tiempo P�del', 1, (SELECT Id FROM Usuario WHERE Auth0Id = 'dueno-ejemplo-1'), 30000.00, 'https://acdn-us.mitiendanube.com/stores/001/408/700/products/full-360-5-b1a2ca90efa7b1d7c917368620610522-1024-1024.jpeg', 'P�del', 2, 'bg-green-100', 'text-green-800'),
('1114 P�del Club', 1, (SELECT Id FROM Usuario WHERE Auth0Id = 'dueno-ejemplo-1'), 25000.00, 'https://dalviansa.com/wp-content/uploads/2021/06/News_Cancha_de_padel_1.jpeg', 'P�del', 2, 'bg-green-100', 'text-green-800'),

-- Complejos de P�del en ciudad 2 (Buenos Aires)
('Guanches Padel', 2, (SELECT Id FROM Usuario WHERE Auth0Id = 'dueno-ejemplo-1'), 25000.00, 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSR8SrBE9Fuf_xMnbvynk1slE2rBnlkwvYfuQ&s', 'P�del', 2, 'bg-green-100', 'text-green-800');


INSERT INTO horario_cancha (canchaid, dia_semana, hora_inicio, hora_fin, disponible)
SELECT 
    c.id AS canchaid,
    d.dia_semana,
    h.hora_inicio,
    h.hora_fin,
    true
FROM cancha c
CROSS JOIN (
    SELECT generate_series(1, 7) AS dia_semana
) d
CROSS JOIN (
    SELECT 
        make_time(h, 0, 0) AS hora_inicio,
        make_time(h + 1, 0, 0) AS hora_fin
    FROM generate_series(8, 22) h   -- 08:00 a 23:00
) h;
