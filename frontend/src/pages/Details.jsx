import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import "./details.css";
import { useNavigate } from "react-router-dom";

const DetailsPage = () => {
  const navigate = useNavigate();
  const { id } = useParams();
  const [viaje, setViaje] = useState(null);
  const [loading, setLoading] = useState(true);

  const goBack = () => {
    navigate(`/viajes`);
  };

  useEffect(() => {
    fetch(`http://localhost:5118/api/viajes/${id}`)
      .then((res) => res.json())
      .then((data) => {
        setViaje(data);
        setLoading(false);
      })
      .catch((err) => {
        console.error("Error al obtener detalles del viaje:", err);
        setLoading(false);
      });
  }, [id]);

  if (loading) return <p>Cargando...</p>;
  if (!viaje) return <p>No se encontró el viaje.</p>;

  return (
    <div className="detalle-wrapper">
      <h1 className="detalle-titulo">{viaje.destination}</h1>

      <div className="detalle-info-grid">
        <div className="detalle-item">
          <span className="detalle-label">Descripción</span>
          <span className="detalle-valor">{viaje.description}</span>
        </div>

        <div className="detalle-item">
          <span className="detalle-label">Fecha de salida</span>
          <span className="detalle-valor">
            {new Date(viaje.departureDate).toLocaleString()}
          </span>
        </div>

        <div className="detalle-item">
          <span className="detalle-label">Fecha de regreso</span>
          <span className="detalle-valor">
            {new Date(viaje.returnDate).toLocaleString()}
          </span>
        </div>

        <div className="detalle-item">
          <span className="detalle-label">Asientos totales</span>
          <span className="detalle-valor">{viaje.totalSeats}</span>
        </div>

        <div className="detalle-item">
          <span className="detalle-label">Asientos disponibles</span>
          <span className="detalle-valor">{viaje.seatsAvailable}</span>
        </div>

        <div className="detalle-item">
          <span className="detalle-label">Costo por persona</span>
          <span className="detalle-costo">
            ${viaje.costPerPerson.toLocaleString()}
          </span>
        </div>
      </div>

      <div className="detalle-acciones">
        <button className="btn-accion">Editar viaje</button>
        <button className="btn-accion" onClick={goBack}>
          Volver
        </button>
      </div>
    </div>
  );
};

export default DetailsPage;
