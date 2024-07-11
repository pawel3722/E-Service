import React from 'react'
import { useState, useEffect } from "react";
import useAxiosPrivate from "../../hooks/useAxiosPrivate";
import { useNavigate, useLocation, useParams } from "react-router-dom";

function ManageService() {
  const [service, setService] = useState()
  const [serviceTypes, setServiceTypes] = useState([])
  const [allParts, setAllParts] = useState([])
  const [parts, setParts] = useState([])
  const [models, setModels] = useState([])
  const [newStatus, setNewStatus] = useState()
  const [newPart, setNewPart] = useState()
  const axiosPrivate = useAxiosPrivate();
  const navigate = useNavigate();
  const location = useLocation();

  const { id } = useParams()

  async function updateService() {
    var status = newStatus !== service.status ? newStatus : -1
    var partId = newPart ? Number(newPart) : -1

    if (status !== -1) {
      try {
        // make axios post request
        await axiosPrivate.put('api/ApplicationUser/me/services/' + id + '/status',
          JSON.stringify({ status }),
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
    if (partId !== -1) {
      try {
        await axiosPrivate.put('api/Service/' + id,
          JSON.stringify({ partId }),
          {
            headers: { 'Content-Type': 'application/json' },
            withCredentials: true
          }
        );
        alert('Zaktualizowano część!')
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
    const getService = async () => {
      try {
        const response = await axiosPrivate.get('/api/ApplicationUser/me/services')
        console.log(response.data)
        var newService = response.data.find(s => s.id === Number(id))
        setService(newService)
        setNewStatus(newService.status)
        console.log(newService)
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
        var newParts = response.data.filter(p => p.service === null)
        setParts(newParts)
        setAllParts(response.data)
      } catch (err) {
        console.error(err)
        navigate('/log', { state: { from: location }, replace: true })
      }
    }

    getServiceTypes()
    getParts()
    getModels()
    getService()
  }, [])

  return (
    <>

      <div>Usługa:</div>
      {
        service && models && serviceTypes
          ? (
            <p>
              Usługa: {serviceTypes.find((s) => s.id === service.serviceTypeId) ? serviceTypes.find(s => s.id === service.serviceTypeId).name : ""},
              Cena: {service.servicePrice},
              Aktualna część: {!service.partId ? "Brak" : allParts.find(p => p.id === service.partId) && models.find(m => m.id === allParts.find(p => p.id === service.partId).modelId)
                ? <label>{models.find(m => m.id === allParts.find(p => p.id === service.partId).modelId).name} #{allParts.find(p => p.id === service.partId).serialNumber} </label> : "XD"},
              Nowa część:
              <select onChange={(e) => setNewPart(e.target.value)}>
                <option value={-1}>Bez zmian</option>
                {
                  parts && models ? parts.map(p =>
                    <option value={p.id}>{models.find(m => m.id === p.modelId).name}, {p.serialNumber}</option>
                  )
                    : ""
                }
              </select>
              Status:
              <select onChange={(e) => setNewStatus(e.target.value)}>
                {service.status < 2 ? <option value={1}>Przypisano pracownika</option> : ""}
                {service.status < 3 ? <option value={2}>Oczekiwanie na część</option> : ""}
                <option value={3}>Ukończono</option>
              </select>
              <button onClick={() => updateService()}>Zapisz</button>
            </p>
          )
          : <p>Ładowanie...</p>
      }

    </>
  )
}

export default ManageService