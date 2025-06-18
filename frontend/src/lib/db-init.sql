-- Tabla de viajes
CREATE TABLE trips (
  id SERIAL PRIMARY KEY,
  destination VARCHAR(255) NOT NULL,
  departure_date TIMESTAMP NOT NULL,
  return_date TIMESTAMP NOT NULL,
  total_seats INTEGER NOT NULL CHECK (total_seats >= 0),
  seats_available INTEGER NOT NULL CHECK (seats_available >= 0),
  cost_per_person NUMERIC(10, 2) NOT NULL CHECK (cost_per_person >= 0),
  description TEXT,
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Participantes
CREATE TABLE participants (
  id SERIAL PRIMARY KEY,
  trip_id INTEGER NOT NULL REFERENCES trips(id) ON DELETE CASCADE,
  name VARCHAR(255) NOT NULL,
  email VARCHAR(255),
  phone VARCHAR(50),
  paid_amount NUMERIC(10, 2) DEFAULT 0 CHECK (paid_amount >= 0),
  payment_complete BOOLEAN DEFAULT FALSE,
  notes TEXT,
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Vista resumen de viajes
CREATE VIEW trip_summaries AS
SELECT 
  t.id,
  t.destination,
  t.departure_date,
  t.return_date,
  t.total_seats,
  t.seats_available,
  t.cost_per_person,
  COUNT(p.id) AS enrolled_count,
  COALESCE(SUM(p.paid_amount), 0) AS total_paid,
  (COUNT(p.id) * t.cost_per_person - COALESCE(SUM(p.paid_amount), 0)) AS amount_remaining
FROM trips t
LEFT JOIN participants p ON t.id = p.trip_id
GROUP BY t.id;

-- Administradores
CREATE TABLE administrators (
  id SERIAL PRIMARY KEY,
  username VARCHAR(50) UNIQUE NOT NULL,
  password_hash VARCHAR(255) NOT NULL, -- hash bcrypt
  email VARCHAR(255) UNIQUE NOT NULL,
  full_name VARCHAR(100) NOT NULL,
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  last_login TIMESTAMP,
  is_active BOOLEAN DEFAULT TRUE
);

-- Sesiones (opcional)
CREATE TABLE sessions (
  id SERIAL PRIMARY KEY,
  administrator_id INTEGER NOT NULL REFERENCES administrators(id),
  session_token VARCHAR(255) UNIQUE NOT NULL,
  expires_at TIMESTAMP NOT NULL,
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);


--7CkUAgVdkbHmi6Ii -- Índices para mejorar el rendimiento

-- Insertar viajes
INSERT INTO trips (destination, departure_date, return_date, total_seats, seats_available, cost_per_person, description)
VALUES
  ('Buenos Aires - Córdoba', '2025-07-15 08:00', '2025-07-18 18:00', 50, 45, 10000.00, 'Viaje al templo de Córdoba'),
  ('Buenos Aires - Santiago', '2025-08-01 06:00', '2025-08-05 20:00', 40, 35, 12000.00, 'Templo de Santiago de Chile'),
  ('Buenos Aires - Montevideo', '2025-09-10 07:00', '2025-09-12 21:00', 30, 25, 8000.00, 'Templo de Montevideo');

-- Insertar participantes
INSERT INTO participants (trip_id, name, email, phone, paid_amount, payment_complete, notes)
VALUES
  (1, 'Juan Pérez', 'juan@example.com', '1155555555', 5000.00, false, 'Pagará el resto en cuotas'),
  (1, 'María García', 'maria@example.com', '1144444444', 10000.00, true, ''),
  (2, 'Carlos López', 'carlos@example.com', '1133333333', 6000.00, false, 'Pago parcial');

-- Insertar administrador de ejemplo (usando una contraseña hasheada con bcrypt)
-- Contraseña: admin123
INSERT INTO administrators (username, password_hash, email, full_name)
VALUES ('admin', '$2a$12$AbCDEFGHIJKLMNOPQRSTuvuxXXXXXyyyyyZzzzzzzYkUYgFYpgW0vAq', 'admin@viajes.com', 'Administrador Ejemplo');
