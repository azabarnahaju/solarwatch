import React from 'react'
import "./Form.css";

const Form = ({ date, setDate, city, setCity, sunMovement, setSunMovement, handleSubmit }) => {
  return (
    <form id="solar-movement-form" className="d-flex justify-content-center">
      <div>
        {sunMovement ? (
          <div>
            <label htmlFor="date" className="sun-form-label form-label ms-2">
              Select a date (optional)
            </label>
            <input
              className="sun-form-input form-control"
              name="date"
              type="date"
              value={date}
              onChange={(e) => setDate(e.target.value)}
            />
          </div>
        ) : (
          <></>
        )}
        <div>
          <label htmlFor="city" className="sun-form-label form-label ms-2">
            Type a city
          </label>
          <input
            className="sun-form-input form-control"
            required={true}
            name="city"
            type="text"
            value={city}
            onChange={(e) => setCity(e.target.value)}
          />
        </div>
        {sunMovement ? (
          <div>
            <label
              htmlFor="sun-movement"
              className="sun-form-label form-label ms-2"
            >
              Select sun movement
            </label>
            <select
              className="sun-form-input form-select"
              name="sun-movement"
              id="sun-movement"
              value={sunMovement}
              onChange={(e) => setSunMovement(e.target.value)}
            >
              <option value="sunrise">Sunrise</option>
              <option value="sunset">Sunset</option>
            </select>
          </div>
        ) : (
          <></>
        )}
        <div className="d-flex justify-content-center">
          <button
            className="btn solar-form-btn mb-3"
            onClick={(e) => handleSubmit(e)}
          >
            Get data
          </button>
        </div>
      </div>
    </form>
  );
}

export default Form