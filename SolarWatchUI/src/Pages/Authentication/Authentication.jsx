import "./Authentication.css";
import SignIn from "../../Components/SignIn/SignIn";
import Register from "../../Components/Register/Register";

const Authentication = () => {
    return (
      <div className="auth-content d-flex justify-content-evenly mx-5">
        <div className="login-container">
          <h2 className="auth-title">Already a user? Log in here!</h2>
          <SignIn />
        </div>
        <div className="me-5">
          <h2 className="auth-title">Join our community!</h2>
          <Register />
        </div>
      </div>
    );
}

export default Authentication;