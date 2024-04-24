import ProtectedRoute from "../../Components/ProtectedRoute/ProtectedRoute";
import Form from "../../Components/Form/Form";
import { useEffect, useState, useContext } from "react";
import { useNavigate } from "react-router-dom";
import baseUrl from "../../Utils/baseUrl";
import { removeToken } from "../../Services/AuthenticationService";
import UserContext from "../../Contexts/UserContext";
import { getTodayFormatted } from "../../Utils/HelperFunctions";
import "./SolarMovement.css";

const defaultSolarData = {
  sunMovement: "sunrise",
  date: "",
  city: ""
}

const SolarMovement = () => {
  const [sunMovement, setSunMovement] = useState("sunrise");
  const [date, setDate] = useState("");
  const [city, setCity] = useState("");
  const [info, setInfo] = useState("");
  const [solarData, setSolarData] = useState(defaultSolarData);
  const navigate = useNavigate();
  const { setCurrentUser } = useContext(UserContext);

  const logoutUser = () => {
    setCurrentUser(null);
    removeToken();
    navigate("/")
  };

  useEffect(() => {}, [solarData]);

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (city === "" || city === null) {
      console.log("Missing city")
      return;
    }

    setSolarData({ sunMovement: sunMovement, date: date, city: city })

    const currentDate = getTodayFormatted();
    const url = `${baseUrl}/${sunMovement}/get${
      sunMovement
    }ondate?cityName=${city}&date=${
      date ? date : currentDate
    }`;
    
    const response = await fetch(url, {
      method: "GET",
      headers: {
        Authorization: `Bearer ${localStorage.getItem("userToken")}`,
      },
    });

    if (response.ok) {
      console.log("Info retrieved successfully.");
      const result = await response.json();
      setInfo(result.time);
    } else {
      console.log("Error getting information.");
    }
    setDate("")
    setCity("")
    setSunMovement("sunrise")
  };

  return (
    <ProtectedRoute>
      <div className="d-flex justify-content-between m-3">
        <button className="nav-btn" onClick={() => navigate("/")}>
          Home
        </button>
        <button className="nav-btn" onClick={logoutUser}>Logout</button>
      </div>
      <div className="sun-data-content d-flex justify-content-around">
        <Form date={date} setDate={setDate} city={city} setCity={setCity} sunMovement={sunMovement} setSunMovement={setSunMovement} handleSubmit={handleSubmit}/>
        <div className="sun-data-container">
          {info ? (
            <div>
              <h3>
                {solarData.city} {solarData.sunMovement} data for{" "}
                {solarData.date ? solarData.date.replaceAll("-", "/") : "today"}:
              </h3>
              <h5 className="mt-3">{info}</h5>
            </div>
          ) : (
            <h3>No data</h3>
          )}
        </div>
      </div>
    </ProtectedRoute>
  );
};

export default SolarMovement;
