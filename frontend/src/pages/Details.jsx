import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import "./details.css";
import { useNavigate } from "react-router-dom";
import Spinner from "../components/Spinner/Spinner";

const DetailsPage = () => {
  const navigate = useNavigate();
  const { id } = useParams();
  const [viaje, setViaje] = useState(null);
  const [participantes, setParticipantes] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [searchTerm, setSearchTerm] = useState("");

  const goBack = () => {
    navigate(`/viajes`);
  };

  const editTrip = () => {
    alert("Editar viaje no implementado");
  };

  const addParticipant = () => {
    alert("Agregar participante no implementado");
  };

  const filteredParticipantes = participantes.filter((p) =>
    p.name?.toLowerCase().includes(searchTerm.toLowerCase())
  );

  useEffect(() => {
    const fetchViajeData = async () => {
      try {
        setLoading(true);
        setError(null);

        console.log("Fetching viaje with ID:", id);

        if (!id) {
          throw new Error("ID del viaje no encontrado en la URL");
        }
        const viajeResponse = await fetch(
          `http://localhost:5118/api/viajes/${id}`
        );

        console.log("Viaje response status:", viajeResponse.status);

        if (!viajeResponse.ok) {
          if (viajeResponse.status === 404) {
            throw new Error("Viaje no encontrado");
          }
          throw new Error(`Error del servidor: ${viajeResponse.status}`);
        }

        const viajeData = await viajeResponse.json();
        console.log("Viaje data:", viajeData);
        setViaje(viajeData);

        const participantesResponse = await fetch(
          `http://localhost:5118/api/viajes/${id}/participantes`
        );

        console.log(
          "Participantes response status:",
          participantesResponse.status
        );

        if (!participantesResponse.ok) {
          console.warn(
            "Error al obtener participantes, pero continuando con viaje"
          );
          setParticipantes([]);
        } else {
          const participantesData = await participantesResponse.json();
          console.log("Participantes data:", participantesData);
          setParticipantes(participantesData);
        }
      } catch (err) {
        console.error("Error al obtener datos:", err);
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };

    fetchViajeData();
  }, [id]);

  const formatDate = (dateString) => {
    try {
      if (!dateString) return "Fecha no disponible";
      const date = new Date(dateString);
      if (isNaN(date.getTime())) return "Fecha inválida";
      return date.toLocaleString("es-ES", {
        year: "numeric",
        month: "2-digit",
        day: "2-digit",
        hour: "2-digit",
        minute: "2-digit",
      });
    } catch (err) {
      console.error("Error al formatear fecha:", err);
      return "Error en fecha";
    }
  };

  const formatCurrency = (amount) => {
    try {
      if (amount === null || amount === undefined) return "$0";
      return `$${Number(amount).toLocaleString("es-ES", {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2,
      })}`;
    } catch (err) {
      console.error("Error al formatear moneda:", err);
      return "$0";
    }
  };

  if (loading) return <Spinner />;

  if (error) {
    return (
      <div className="detalle-wrapper">
        <div className="error-container">
          <h2>Error</h2>
          <p>{error}</p>
          <button className="btn-accion" onClick={goBack}>
            Volver a viajes
          </button>
        </div>
      </div>
    );
  }

  if (!viaje) {
    return (
      <div className="detalle-wrapper">
        <div className="error-container">
          <h2>Viaje no encontrado</h2>
          <p>No se pudo encontrar el viaje solicitado.</p>
          <button className="btn-accion" onClick={goBack}>
            Volver a viajes
          </button>
        </div>
      </div>
    );
  }

  return (
    <div className="detalle-wrapper">
      <h1 className="detalle-titulo">
        {viaje.destination || "Destino no especificado"}
      </h1>

      <div className="detalle-info-grid">
        <div className="detalle-item">
          <span className="detalle-label">Descripción</span>
          <span className="detalle-valor">
            {viaje.description || "Sin descripción"}
          </span>
        </div>

        <div className="detalle-item">
          <span className="detalle-label">Fecha de salida</span>
          <span className="detalle-valor">
            {formatDate(viaje.departureDate)}
          </span>
        </div>

        <div className="detalle-item">
          <span className="detalle-label">Fecha de regreso</span>
          <span className="detalle-valor">{formatDate(viaje.returnDate)}</span>
        </div>

        <div className="detalle-item">
          <span className="detalle-label">Asientos totales</span>
          <span className="detalle-valor">{viaje.totalSeats || 0}</span>
        </div>

        <div className="detalle-item">
          <span className="detalle-label">Asientos disponibles</span>
          <span className="detalle-valor">{viaje.seatsAvailable || 0}</span>
        </div>

        <div className="detalle-item">
          <span className="detalle-label">Costo por persona</span>
          <span className="detalle-costo">
            {formatCurrency(viaje.costPerPerson)}
          </span>
        </div>
      </div>

      <div className="detalle-acciones">
        <button className="btn-accion" onClick={goBack}>
          Volver
        </button>
        <button className="btn-accion" onClick={editTrip}>
          Editar viaje
        </button>
        <button className="btn-accion" onClick={addParticipant}>
          Agregar participante
        </button>
      </div>

      <div>
        <h2 className="detalle-subtitulo">
          Participantes ({participantes.length})
        </h2>

        <input
          type="text"
          placeholder="Buscar por nombre..."
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
          className="input-busqueda"
        />

        {filteredParticipantes.length === 0 ? (
          <div className="no-participantes">
            <p>No hay participantes que coincidan con la búsqueda.</p>
          </div>
        ) : (
          <div className="tabla-container">
            <table className="tabla-participantes">
              <thead>
                <tr>
                  <th>Nombre</th>
                  <th>Email</th>
                  <th>Teléfono</th>
                  <th>Monto Pagado</th>
                  <th>Pago Completo</th>
                  <th>Notas</th>
                  <th>Acciones</th>
                </tr>
              </thead>
              <tbody>
                {filteredParticipantes.map((p) => (
                  <tr key={p.id}>
                    <td>{p.name || "Sin nombre"}</td>
                    <td>{p.email || "-"}</td>
                    <td>{p.phone || "-"}</td>
                    <td>{formatCurrency(p.paidAmount)}</td>
                    <td>
                      <span
                        className={`payment-status ${
                          p.paymentComplete ? "complete" : "incomplete"
                        }`}
                      >
                        {p.paymentComplete ? "Sí" : "No"}
                      </span>
                    </td>
                    <td>{p.notes || "-"}</td>
                    <td>
                      <button
                        className="btn-editar-participante"
                        onClick={() => alert(`Editar participante ${p.name}`)}
                      >
                        Editar
                      </button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>
    </div>
  );
};

export default DetailsPage;
