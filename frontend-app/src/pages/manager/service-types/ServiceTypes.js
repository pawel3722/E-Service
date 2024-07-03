import React from 'react'
import { useState, useEffect } from "react";
import useAxiosPrivate from "../../../hooks/useAxiosPrivate";
import { useNavigate, useLocation } from "react-router-dom";

function ServiceTypes() {
  const [serviceTypes, setServiceTypes] = useState([])
  const axiosPrivate = useAxiosPrivate();
  const navigate = useNavigate();
  const location = useLocation();

  useEffect(() => {
    const getServiceTypes = async () => {
      try {
        const response = await axiosPrivate.get('/api/ServiceType')
        console.log(response.data)
        setServiceTypes(response.data)
      } catch (err) {
        console.error(err)
        navigate('/log', { state: { from: location }, replace: true })
      }
    }
    getServiceTypes()
  }, [])

  return (
    <>

      <div>Możliwe usługi:</div>
      <button onClick={() => navigate('/user/new-service-type', { state: { from: location }, replace: false } )}>Dodaj</button>
      {
        serviceTypes
          ? (
            serviceTypes.map((s) => (
              <p>
                Usługa: {s.name}<br></br>
                Przedział cen: {s.minPrice} - {s.maxPrice}<br></br>
                Urządzenie: {s.deviceType} <br></br>
                <button onClick={() => navigate('/user/edit-service-type/' + s.id, { state: { from: location }, replace: false } )}>Edytuj</button>
                <button onClick={() => navigate('/user/delete-service-type/' + s.id, { state: { from: location }, replace: false })}>Usuń</button>
              </p>
            ))
          )
          : <p>Ładowanie...</p>
      }

    </>
  )
}

export default ServiceTypes