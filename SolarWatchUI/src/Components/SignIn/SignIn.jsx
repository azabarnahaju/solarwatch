import { useContext, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import UserContext from "../../Contexts/UserContext";
import "./SignIn.css";
import baseUrl from "../../Utils/baseUrl";

const SignIn = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [success, setSuccess] = useState(null);
  const navigate = useNavigate();
  const { setCurrentUser } = useContext(UserContext);

  const handleLogin = async (e) => {
    e.preventDefault();

    const response = await fetch(`${baseUrl}/auth/login`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ email, password }),
    });

    if (response.ok) {
      setSuccess(true);
      const result = await response.json();
      localStorage.setItem("userToken", result.token);
      setCurrentUser({ email: result.email, username: result.userName });
      console.log("Successful login!");
      navigate("/");
    } else {
      setSuccess(false);
      console.log("Login failed.");
      setEmail("");
      setPassword("");
    }
    setSuccess(null);
  };

  return (
    <div>
      <form id="login-form" className="d-flex justify-content-center">
        <div>
          <div className="my-3">
            <label htmlFor="email" className="login-form-label form-label ms-2">
              Email
            </label>
            <input
              className="login-form-input form-control"
              name="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              placeholder="Email"
              type="email"
            />
          </div>
          <div className="my-3">
            <label
              htmlFor="password"
              className="login-form-label form-label ms-2"
            >
              Password
            </label>
            <input
              className="login-form-input form-control"
              name="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              placeholder="Password"
              type="password"
            />
          </div>
          <div className="d-flex justify-content-center">
            <button
              onClick={(e) => handleLogin(e)}
              className="btn login-form-btn mb-3"
            >
              Login
            </button>
          </div>
        </div>
      </form>
      {success === true ? (
        <div className="alert alert-success" role="alert">
          Successful login!
        </div>
      ) : success === false ? (
        <div className="alert alert-danger" role="alert">
          Login failed!
        </div>
      ) : (
        <></>
      )}
    </div>
  );
};

export default SignIn;
