import React, { lazy, Suspense } from "react";
import { createBrowserRouter, RouterProvider } from "react-router-dom";
import "./App.css"; 
import Layout from "./layouts/Layout.tsx";
import GenderSetup from "./pages/UserManagement/GenderSetup/GenderSetup.tsx";
import UserProfile from "./pages/UserManagement/UserProfile/UserProfile.tsx";

const router = createBrowserRouter([
  {
    path: "/",
    element: <Layout />,
    children: [
      {
        path: "/user-management/genders",
        element: <GenderSetup />
      },
      {
        path: "/user-management/users",
        element: <UserProfile />
      }
    ],
    errorElement: (
      <Suspense fallback={<div>Loading...</div>}>
        <div>Page Not Found.</div>
      </Suspense>
    )
  },
]);

function App(): React.JSX.Element {
  return <RouterProvider router={router} />;
}

export default App;