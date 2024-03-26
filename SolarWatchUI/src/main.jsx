import React from 'react';
import ReactDOM from 'react-dom/client';
import { createBrowserRouter, BrowserRouter } from 'react-router-dom';
import 'bootstrap/dist/css/bootstrap.min.css';
import "./index.css";
import Navbar from "./Pages/Navbar/Navbar"
import Home from './Pages/Home/Home'
import Register from './Components/Register/Register'
import ErrorPage from './Pages/ErrorPage/ErrorPage';
import SignIn from './Components/SignIn/SignIn';
import SolarMovement from './Pages/SolarMovement/SolarMovement';
import { UserProvider } from './Contexts/UserContext';
import App from './App';

const router = createBrowserRouter([
  {
    path: "/",
    element: <Navbar />,
    errorElement: <ErrorPage />,
    children: [
      {
        path: "/",
        element: <Home />,
      },
      {
        path: "signin",
        element: <SignIn />,
      },
      {
        path: "register",
        element: <Register />,
      },
      {
        path: "solar-watch",
        element: <SolarMovement />,
      }
    ],
  },
]);

const root = ReactDOM.createRoot(document.getElementById("root"))
root.render(
  <React.StrictMode>
    <BrowserRouter>
      <UserProvider>
        <App />
      </UserProvider>
    </BrowserRouter>
  </React.StrictMode>
);
