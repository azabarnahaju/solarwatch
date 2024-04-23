import "./Home.css";
import dots from "../../assets/white-dots-cut.png";
import { useNavigate } from "react-router-dom";
import { useContext } from "react";
import UserContext from "../../Contexts/UserContext";
import { removeToken } from "../../Services/AuthenticationService";
import { FaMoon } from "react-icons/fa";
import { IoMdSunny } from "react-icons/io";
import { IconContext } from "react-icons";

const Home = () => {
  const navigate = useNavigate();
  const { currentUser, setCurrentUser } = useContext(UserContext);

  const logoutUser = () => {
    setCurrentUser(null);
    removeToken();
  }

  return (
    <div className="home-content">
      <div className="img-container">
        <img src={dots} alt="white-dots" className="dot-image img-fluid" />
      </div>
      <h1 className="home-title">SolarWatch</h1>
      <div className="btn-container d-flex justify-content-evenly pt-5 mt-3">
        {currentUser ? (
          <button className="auth-btn mt-5" onClick={logoutUser}>
            Logout
          </button>
        ) : (
          <button className="auth-btn mt-5" onClick={() => navigate("/auth")}>
            SIGN IN | REGISTER
          </button>
        )}
        <div>
          <button
            className="solar-data-btn mt-5 mx-2 text-align-center"
            onClick={() => navigate("/solar-watch")}
          >
            GET{" "}
            <IconContext.Provider value={{ size: "1.2em" }}>
              <IoMdSunny className="mb-1 mx-1" />
            </IconContext.Provider>{" "}
            DATA
          </button>
          <button
            className="solar-data-btn mt-5 mx-2"
            onClick={() => navigate("/moon-data")}
          >
            <span>
              GET <FaMoon className="mb-1 mx-1" /> DATA
            </span>
          </button>
        </div>
      </div>
    </div>
  );
};

export default Home;
