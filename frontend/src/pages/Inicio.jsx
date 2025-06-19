import React from "react";
import "./Inicio.css";

export default function Inicio() {
  return (
    <div className="inicioHero">
      <img
        src="/public/templo.webp"
        alt="Templo SUD"
        className="inicioBackground"
      />
      <div className="inicioOverlay">
        <h1 className="inicioTitle">Prepará tu próximo viaje al templo</h1>
        <p className="inicioText">
          Planificá experiencias sagradas con orden y propósito.
        </p>
        <a href="/crear" className="inicioButton">
          Crear un nuevo viaje
        </a>
      </div>
    </div>
  );
}
