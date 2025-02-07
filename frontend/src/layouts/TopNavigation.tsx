import React, { memo } from "react";

const TopNavigation = memo((): React.JSX.Element => {
  return (
    <nav className="navbar navbar-expand-md navbar-light bg-white shadow-sm">
      <div className="container">
        <i className="bi bi-person-lines-fill me-2" style={{ fontSize: "20px" }}></i>
        <span className="navbar-brand">{import.meta.env[`VITE_REACT_APP_NAME_${'DEV'}`]}</span>
        <button
          className="navbar-toggler"
          type="button"
          data-bs-toggle="collapse"
          data-bs-target="#navbarSupportedContent"
          aria-controls="navbarSupportedContent"
          aria-expanded="false"
          aria-label="Toggle navigation"
        ><span className="navbar-toggler-icon"></span>
        </button>
        <div
          className="collapse navbar-collapse"
          id="navbarSupportedContent"
        >
          {/* Left Side Of Navbar */}
          <ul className="navbar-nav me-auto"></ul>
          {/* Right Side Of Navbar */}
          <ul className="navbar-nav ms-auto">
            <li className="nav-item dropdown">
              <a
                className="nav-link dropdown-toggle tt-capitalize"
                id="navbarDropdown"
                href="#"
                role="button"
                data-bs-toggle="dropdown"
                aria-haspopup="true"
                aria-expanded="false"
              ></a>
            </li>
          </ul>
        </div>
      </div>
    </nav>
  );
});

export default TopNavigation;