import React, { createContext, useState } from "react";
import { useNavigate } from "react-router-dom";
import { removeToken, validateToken } from "../Services/AuthenticationService";
import { useEffect } from "react";

export const UserContext = createContext({
  currentUser: null,
  setCurrentUser: () => {}
});

export const UserProvider = ({children}) => {
    const [currentUser, setCurrentUser] = useState(null);
    const value = { currentUser, setCurrentUser };
    const navigate = useNavigate();

    const getAuthStatus = () => {
      try {
        const tokenValidation = validateToken();
        if (!tokenValidation){
          removeToken();
          setCurrentUser(null);
        } else{
          setCurrentUser({ username: "userXY" })
        }
      } catch (error) {
        removeToken();
        setCurrentUser(null);
        console.log("Error during authetication process.", error)
      }
    }

    useEffect(() => {
      getAuthStatus()
    }, [navigate]);

    return (
      <UserContext.Provider value={value}>
        {children}
      </UserContext.Provider>
    );
}

export default UserContext;