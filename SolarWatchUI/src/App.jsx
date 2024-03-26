import { Route, Routes } from 'react-router-dom'
import './App.css'
import Home from './Pages/Home/Home'
import SolarMovement from './Pages/SolarMovement/SolarMovement'
import Authentication from './Pages/Authentication/Authentication'

function App() {
  return (
    <div>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/auth" element={<Authentication/>}/>
        <Route path="/solar-watch" element={<SolarMovement />}/>
      </Routes>
    </div>
  )
}

export default App
