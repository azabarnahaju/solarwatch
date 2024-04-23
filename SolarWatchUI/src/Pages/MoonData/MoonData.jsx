import React from 'react'
import "./MoonData.css";
import baseUrl from '../../Utils/baseUrl';
import ProtectedRoute from '../../Components/ProtectedRoute/ProtectedRoute'
import Form from '../../Components/Form/Form';
import { useState, useContext, useEffect } from "react";
import { useNavigate } from 'react-router-dom';
import UserContext from '../../Contexts/UserContext';
import MoonPhase from '../../Components/MoonPhase/MoonPhase';
import { removeToken } from "../../Services/AuthenticationService";

const MoonData = () => {
    const [moonData, setMoonData] = useState("");
    const [city, setCity] = useState("");
    const [info, setInfo] = useState("");
    const navigate = useNavigate();
    const { setCurrentUser } = useContext(UserContext);

    const logoutUser = () => {
    setCurrentUser(null);
    removeToken();
    navigate("/");
    };

    useEffect(() => {}, [moonData]);

    const handleSubmit = async (e) => {
      e.preventDefault();

      if (city === "" || city === null) {
        console.log("Missing city");
        return;
      }

      setMoonData({ city: city });

      const url = `${baseUrl}/moon/getMoonData?cityName=${city}`;

      const response = await fetch(url, {
        method: "GET",
        headers: {
          Authorization: `Bearer ${localStorage.getItem("userToken")}`,
        },
      });

      if (response.ok) {
        console.log("Info retrieved successfully.");
        const result = await response.json();
        console.log(result);
        setInfo(result);
      } else {
        console.log("Error getting information.");
      }
      setCity("");
    };

  return (
    <ProtectedRoute>
      <div className="d-flex justify-content-between m-3">
        <button className="nav-btn" onClick={() => navigate("/")}>
          Home
        </button>
        <button className="nav-btn" onClick={logoutUser}>
          Logout
        </button>
      </div>
      <div className="sun-data-content d-flex justify-content-center"></div>
      <Form
        date={null}
        setDate={null}
        city={city}
        setCity={setCity}
        sunMovement={null}
        setSunMovement={null}
        handleSubmit={handleSubmit}
      />
      <div className="sun-data-container">
        {info ? (
          <table>
            <tbody>
              <tr>
                <td>Current phase</td>
                <td>
                  {info.currentPhase}
                  <MoonPhase phase={info.currentPhase}></MoonPhase>
                </td>
              </tr>
              <tr>
                <td>Next phase</td>
                <td>
                  {info.nextPhase}
                  <MoonPhase phase={info.nextPhase}></MoonPhase>
                </td>
              </tr>
              <tr>
                <td>Next begins at</td>
                <td>{info.nextPhaseTime}</td>
              </tr>
              <tr>
                <td>Moonrise</td>
                <td>{info.moonRise}</td>
              </tr>
              <tr>
                <td>Moonset</td>
                <td>{info.moonSet}</td>
              </tr>
              <tr>
                <td>Moon fraction</td>
                <td>{Math.round(info.moonFraction * 100)}%</td>
              </tr>
            </tbody>
          </table>
        ) : (
          <h3>No data</h3>
        )}
      </div>
    </ProtectedRoute>
  );
}

export default MoonData