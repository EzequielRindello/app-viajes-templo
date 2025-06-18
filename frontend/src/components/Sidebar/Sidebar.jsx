import "./sidebar.css";

const navItems = [
  { label: "Inicio", href: "/" },
  { label: "Viajes", href: "/viajes" },
  { label: "Crear viaje", href: "/crear" },
  { label: "Tu Cuenta", href: "/cuenta" },
];

export default function Sidebar({ activeRoute, onNavigate }) {
  const currentYear = new Date().getFullYear();

  const handleNavClick = (href, event) => {
    event.preventDefault();
    onNavigate(href);
  };

  return (
    <aside className="sidebar">
      <h2 className="sidebarTitle">🏛️ TEMPLIFY</h2>
      <nav className="nav">
        {navItems.map((item) => (
          <a
            key={item.href}
            href={item.href}
            onClick={(e) => handleNavClick(item.href, e)}
            className={`navLink ${activeRoute === item.href ? "active" : ""}`}
          >
            {item.label}
          </a>
        ))}
      </nav>
      <div className="sidebarFooter">
        <p>© {currentYear} Viajes Templo</p>
        <p>Desarrollado por Ezequiel Rindello</p>
      </div>
    </aside>
  );
}
