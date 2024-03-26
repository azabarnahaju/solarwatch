import { Navigate } from "react-router-dom";

const ProtectedRoute = ({ children }) => {
    if (!localStorage.getItem("userToken")){
        return < Navigate to="/auth" replace />;
    }

    return children ? children : <Outlet />;
}

export default ProtectedRoute;