import React from 'react'
import { useState, useEffect } from "react";
import useAxiosPrivate from "../../hooks/useAxiosPrivate";
import { useNavigate, useLocation } from "react-router-dom";

function ServicemanServices() {
  const [serviceTypes, setServiceTypes] = useState([])
  const [services, setServices] = useState([])
  const [parts, setParts] = useState([])
  const [models, setModels] = useState([])
  const [newStatus, setNewStatus] = useState()
  const [part, setPart] = useState()
  const axiosPrivate = useAxiosPrivate();
  const navigate = useNavigate();
  const location = useLocation();

  async function updateStatus(id) {
    var status = newStatus
    var partId = part ? Number(part) : -1

    if (part !== -1) {
      try {
        // make axios post request
        await axiosPrivate.put('api/ApplicationUser/me/services/' + id + '/status',
          JSON.stringify({ status }),
          {
            headers: { 'Content-Type': 'application/json' },
            withCredentials: true
          }
        );
        alert('Zaktualizowano usługę!')
      } catch (error) {
        console.log(error)
        alert(error)
      }
    }
    else {
      try {
        await axiosPrivate.put('api/ApplicationUser/me/services/' + id + '/status',
          JSON.stringify({ status, partId }),
          {
            headers: { 'Content-Type': 'application/json' },
            withCredentials: true
          }
        );
        alert('Zaktualizowano status!')
      } catch (error) {
        console.log(error)
        alert(error)
      }
    }
  }


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
                  Usługa: {serviceTypes.find((st) => st.id === s.serviceTypeId) ? serviceTypes.find((st) => st.id === s.serviceTypeId).name : ""} ,
                  Cena: {s.servicePrice},
                  Część:
                  <select onChange={(e) => setPart(e.target.value)}>
                    <option value={-1}>Brak</option>
                    {
                      parts && models ? parts.map(p =>
                        <option value={p.id}>{models.find(m => m.id === p.modelId).name}, {p.serialNumber}</option>
                      )
                        : ""
                    }
                  </select>
                  Status:
                  <select onChange={(e) => setNewStatus(e.target.value)}>
                    {s.status < 2 ? (<option value={1}>Przypisano pracownika</option>) : ""}
                    {s.status < 3 ? <option value={2}>Oczekiwanie na część</option> : ""}
                    <option value={3}>Ukończono</option>
                  </select>
                  <button onClick={() => updateStatus(s.id)}>Zapisz</button>
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