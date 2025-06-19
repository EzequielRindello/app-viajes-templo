import React from "react";
import {
  BrowserRouter as Router,
  Routes,
  Route,
  useNavigate,
  useLocation,
} from "react-router-dom";
import Sidebar from "./components/SideBar/SideBar";
import "./App.css";
import ViajesPage from "./pages/Viajes";
import DetailsPage from "./pages/Details";
import InicioPage from "./pages/Inicio";

function Inicio() {
  return <InicioPage />;
}

function Viajes() {
  return <ViajesPage />;
}

function CrearViaje() {
  return <h2>Crear Viaje</h2>;
}

function TuCuenta() {
  return <h2>Cuenta</h2>;
}

function Details() {
  return <DetailsPage />;
}

function AppLayout() {
  const navigate = useNavigate();
  const location = useLocation();

  return (
    <div className="app-layout">
      <Sidebar activeRoute={location.pathname} onNavigate={navigate} />
      <main className="main-content">
        <Routes>
          <Route path="/" element={<Inicio />} />
          <Route path="/viajes" element={<Viajes />} />
          <Route path="/crear" element={<CrearViaje />} />
          <Route path="/cuenta" element={<TuCuenta />} />
          <Route path="/details/:id" element={<Details />} />
        </Routes>
      </main>
    </div>
  );
}

export default function App() {
  return (
    <Router>
      <AppLayout />
    </Router>
  );
}
