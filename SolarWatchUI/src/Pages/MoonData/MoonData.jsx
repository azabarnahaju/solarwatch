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
import { getTodayFormatted } from "../../Utils/HelperFunctions";

const MoonData = () => {
    const [moonData, setMoonData] = useState("");
    const [city, setCity] = useState("");
    const [date, setDate] = useState("");
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

      const currentDate = getTodayFormatted();
      setMoonData({ city: city, date: date });

      const url = `${baseUrl}/moon/getMoonData?cityName=${city}&date=${date ? date : currentDate}`;

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
      setDate("");
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
      <div className="moon-data-content d-flex justify-content-around">
        <div className='moon-form-container'>
          <Form
            date={date}
            setDate={setDate}
            city={city}
            setCity={setCity}
            sunMovement={null}
            setSunMovement={null}
            handleSubmit={handleSubmit}
          />
        </div>
        <div className="sun-data-container">
          {info ? (
            <div className="inner-info-container">
              <h4>
                {moonData.city} moon information for{" "}
                {moonData.date ? moonData.date.replaceAll("-", "/") : "today"}
              </h4>
              <table className="moon-data-table">
                <tbody>
                  <tr>
                    <td className="variable">Current phase</td>
                    <td className="d-flex justify-content-center text-align-cetner align-items-center">
                      {info.currentPhase}
                      <td className="moon-phase">
                        <MoonPhase phase={info.currentPhase}></MoonPhase>
                      </td>
                    </td>
                  </tr>
                  <tr>
                    <td className="variable">Next phase</td>
                    <td className="d-flex justify-content-center text-align-cetner align-items-center">
                      {info.nextPhase}
                      <td className="moon-phase">
                        <MoonPhase phase={info.nextPhase}></MoonPhase>
                      </td>
                    </td>
                  </tr>
                  <tr>
                    <td className="variable">Next begins at</td>
                    <td>{new Date(info.nextPhaseTime).toLocaleString()} UTC</td>
                  </tr>
                  <tr>
                    <td className="variable">Moonrise</td>
                    <td>{new Date(info.moonRise).toLocaleString()} UTC</td>
                  </tr>
                  <tr>
                    <td className="variable">Moonset</td>
                    <td>{new Date(info.moonSet).toLocaleString()} UTC</td>
                  </tr>
                  <tr>
                    <td className="variable">Moon fraction</td>
                    <td>{Math.round(info.moonFraction * 100)}%</td>
                  </tr>
                </tbody>
              </table>
            </div>
          ) : (
            <h3>No data</h3>
          )}
        </div>
      </div>
    </ProtectedRoute>
  );
}

export default MoonData