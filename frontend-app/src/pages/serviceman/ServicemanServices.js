import React from 'react'
import { useState, useEffect } from "react";
import useAxiosPrivate from "../../hooks/useAxiosPrivate";
import { useNavigate, useLocation } from "react-router-dom";

function ServicemanServices() {
  const [serviceTypes, setServiceTypes] = useState([])
  const [services, setServices] = useState([])
  const [models, setModels] = useState([])
  const [parts, setParts] = useState([])
  const axiosPrivate = useAxiosPrivate();
  const navigate = useNavigate();
  const location = useLocation();

  useEffect(() => {
    const getServiceTypes = async () => {
      try {
        const response = await axiosPrivate.get('/api/ServiceType/')
        console.log(response.data)
        setServiceTypes(response.data)
      } catch (err) {
        console.error(err)
        navigate('/log', { state: { from: location }, replace: true })
      }
    }

    const getServices = async () => {
      try {
        const response = await axiosPrivate.get('/api/ApplicationUser/me/services')
        console.log(response.data)
        setServices(response.data)
      } catch (err) {
        console.error(err)
        navigate('/log', { state: { from: location }, replace: true })
      }
    }

    const getModels = async () => {
      try {
        const response = await axiosPrivate.get('/api/Model')
        console.log(response.data)
        setModels(response.data)
      } catch (err) {
        console.error(err)
        navigate('/log', { state: { from: location }, replace: true })
      }
    }

    const getParts = async () => {
      try {
        const response = await axiosPrivate.get('/api/Part')
        console.log(response.data)
        setParts(response.data)
      } catch (err) {
        console.error(err)
        navigate('/log', { state: { from: location }, replace: true })
      }
    }

    getParts()
    getModels()
    getServices()
    getServiceTypes()
  }, [])

  return (
    <>

      <div>Moje usługi:</div>
      {
        services
          ? (
            <p>
              {services.map((s) =>
                <li>
                  Usługa: {serviceTypes.find((st) => st.id === s.serviceTypeId) ? serviceTypes.find((st) => st.id === s.serviceTypeId).name : ""},
                  Cena: {s.servicePrice},
                  Część: {!s.partId ? "Brak" : parts.find(p => p.id === s.partId) && models.find(m => m.id === parts.find(p => p.id === s.partId).modelId)
                    ? <label>{models.find(m => m.id === parts.find(p => p.id === s.partId).modelId).name} #{parts.find(p => p.id === s.partId).serialNumber} </label> : ""},
                  Status: {s.status === 1 ? "Przypisano pracownika"
                    : s.status === 2 ? "Oczekiwanie na część"
                      : "Ukończono"}
                  <button onClick={() => navigate('/user/manage-service/' + s.id)}>Edytuj</button>
                </li>
              )}
            </p>
          )
          : <p>Ładowanie...</p>
      }

    </>
  )
}

export default ServicemanServices