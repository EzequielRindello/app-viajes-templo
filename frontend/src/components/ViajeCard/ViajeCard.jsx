import React from "react";
import { useNavigate } from "react-router-dom";

const ViajeCard = ({ viaje }) => {
  const navigate = useNavigate();

  const handleClick = () => {
    navigate(`/details/${viaje.id}`);
  };

  return (
    <div className="viaje-card">
      <h2 className="viaje-destino">{viaje.destination}</h2>
      <p className="viaje-descripcion">{viaje.description}</p>
      <p className="viaje-fechas">
        Salida: {new Date(viaje.departureDate).toLocaleDateString()}
        <br />
        Regreso: {new Date(viaje.returnDate).toLocaleDateString()}
      </p>
      <p className="viaje-lugares">
        Lugares disponibles: {viaje.seatsAvailable}/{viaje.totalSeats}
      </p>
      <p className="viaje-costo">${viaje.costPerPerson.toLocaleString()}</p>
      <button className="btn-detalles" onClick={handleClick}>
        Ver detalles
      </button>
    </div>
  );
};

export default ViajeCard;
