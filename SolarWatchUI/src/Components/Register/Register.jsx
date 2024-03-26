import { useState } from "react";
import "./Register.css";
import baseUrl from "../../Utils/baseUrl";

const Register = () => {
  const [email, setEmail] = useState("");
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [success, setSuccess] = useState(null);

  const handleRegister = async (e) => {
    e.preventDefault();

    const response = await fetch(`${baseUrl}/auth/register`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ email, username, password }),
    });

    if (response.ok) {
      setSuccess(true);
      console.log("Successful registration!");
    } else {
      setSuccess(false);
      console.log("Registration failed.");
    }
  };

  return (
    <div>
      <form id="registration-form" className="d-flex justify-content-center">
        <div>
          <div className="my-3">
            <label htmlFor="email" className="reg-form-label form-label ms-2">
              Email
            </label>
            <input
              className="reg-form-input form-control"
              name="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              placeholder="Email"
              type="email"
            />
          </div>
          <div className="my-3">
            <label
              htmlFor="username"
              className="reg-form-label form-label ms-2"
            >
              Username
            </label>
            <input
              className="reg-form-input form-control"
              name="username"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              placeholder="Username"
            />
          </div>
          <div className="my-3">
            <label
              htmlFor="password"
              className="reg-form-label form-label ms-2"
            >
              Password
            </label>
            <input
              className="reg-form-input form-control"
              name="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              placeholder="Password"
              type="password"
            />
          </div>
          <div className="d-flex justify-content-center">
            <button
              className="btn reg-form-btn mb-3"
              onClick={(e) => handleRegister(e)}
            >
              Register
            </button>
          </div>
        </div>
      </form>
      {success === true ? (
        <div className="alert alert-success" role="alert">
          Successful registration!
        </div>
      ) : success === false ? (
        <div className="alert alert-danger" role="alert">
          Registration failed!
        </div>
      ) : (
        <></>
      )}
    </div>
  );
};

export default Register;
