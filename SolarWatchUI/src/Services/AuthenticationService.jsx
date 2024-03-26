import { jwtDecode } from "jwt-decode";

export const removeToken = () => {
    try {
        localStorage.removeItem("userToken")
        return true;
    } catch (error) {
        console.log("Failed to logout")
        return false;
    }
}

export const validateToken = () => {
  try {
    const token = localStorage.getItem("userToken");
    if (!token){
        console.log("Couldn't find token.")
        return false;
    }

    const decodedToken = jwtDecode(token);
    let currentDate = new Date();

    // JWT exp is in seconds
    if (decodedToken.exp * 1000 < currentDate.getTime()) {
      console.log("Token expired.");
      return false;
    } else {
      console.log("Valid token");
      return true;
    }
  } catch (error) {
    console.log("Error while validating token.", error)
    return false;
  }
};