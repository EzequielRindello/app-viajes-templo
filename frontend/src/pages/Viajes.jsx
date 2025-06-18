import React, { useEffect, useState } from "react";
import ViajeCard from "../components/ViajeCard/ViajeCard";
import "./viajes.css";
import Spinner from "../components/Spinner/Spinner";

const Viajes = () => {
  const [viajes, setViajes] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    fetch("http://localhost:5118/api/viajes")
      .then((res) => res.json())
      .then((data) => {
        setViajes(data);
        setLoading(false);
      })
      .catch((err) => {
        console.error("Error al obtener viajes:", err);
        setLoading(false);
        alert("Error al cargar los viajes. Por favor, inténtalo más tarde.");
      });
  }, []);

  return (
    <div className="galeria-container">
      <h1 className="galeria-title">Próximos Viajes</h1>
      {loading ? (
        <Spinner />
      ) : (
        <div className="galeria-grid">
          {viajes.map((viaje) => (
            <ViajeCard key={viaje.id} viaje={viaje} />
          ))}
        </div>
      )}
    </div>
  );
};

export default Viajes;
