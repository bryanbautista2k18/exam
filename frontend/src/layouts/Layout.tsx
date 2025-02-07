import React, { memo } from "react";
import { Outlet } from "react-router-dom";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import TopNavigation from "./TopNavigation.tsx";
import Sidebar from "./Sidebar/Sidebar.tsx";
import Footer from "./Footer.tsx";

const Layout = memo((): React.JSX.Element => {
  return (
    <div className="d-flex" style={{ height: "100vh" }}>
      <Sidebar />
      <div className="d-flex flex-column w-100">
        <TopNavigation />
        <div className="d-flex flex-column flex-grow-1">
          <main className="py-3 flex-grow-1">
            <Outlet />
            <ToastContainer />
          </main>
          <Footer />
        </div>
      </div>
    </div>
  );
});

export default Layout;