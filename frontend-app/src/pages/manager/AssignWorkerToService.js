import React from 'react'
import { useState, useEffect } from "react";
import useAxiosPrivate from "../../hooks/useAxiosPrivate";
import { useNavigate, useLocation } from "react-router-dom";

function AssignManagerToOrder() {
  const [services, setServices] = useState([])
  const [service, setService] = useState()
  const [workers, setWorkers] = useState([])
  const [worker, setWorker] = useState()
  const axiosPrivate = useAxiosPrivate();
  const navigate = useNavigate();
  const location = useLocation();

  async function updateService() {
    var servicemanId = worker ? worker : -1
    var serviceId = service ? service : -1


    if (serviceId !== -1) {
      if (servicemanId !== -1) {
        try {
          // make axios post request
          await axiosPrivate.put('/api/Service/' + serviceId,
            JSON.stringify({ servicemanId }),
            {
              headers: { 'Content-Type': 'application/json' },
              withCredentials: true
            }
          )
          alert("Dodano pracownika!")
        } catch (error) {
          console.log(error)
          alert(error)
        }
      }
      else
        alert("Proszę wybrać pracownika!")
    }
    else
      alert("Proszę wybrać usługę!")
  }

  useEffect(() => {
    const getServices = async () => {
      try {
        const response = await axiosPrivate.get('/api/Service')
        console.log(response.data)
        var pendingServices = response.data.filter(s => s.status === 0)
        setServices(pendingServices)
      } catch (err) {
        console.error(err)
        navigate('/log', { state: { from: location }, replace: true })
      }
    }

    const getWorkers = async () => {
      try {
        const response = await axiosPrivate.get('/api/ApplicationUser/roles')
        console.log(response.data)
        var newWorkers = response.data.find(r => r.name === 'Serviceman').users
        setWorkers(newWorkers)
      } catch (err) {
        console.error(err)
        navigate('/log', { state: { from: location }, replace: true })
      }
    }
    getServices()
    getWorkers()
  }, [])

  return (
    <>

      <div>Przypisz pracownika:</div>
      {
        services && workers
          ? (
            <form onSubmit={() => updateService()}>
              <label>Zamówienie:</label><br></br>
              <select name="order" onChange={(e) => setService(e.target.value)}>
                <option value="-1">Wybierz usługę</option>
                {services.map((s) =>
                  <option value={s.id}>{s.id}, {s.serviceType.name}, {s.serviceType.deviceType}, {s.servicePrice} zł</option>)}
              </select><br></br>
              <label>Menedżer:</label><br></br>
              <select name="manager" onChange={(e) => setWorker(e.target.value)}>
                <option value="-1">Wybierz pracownika</option>
                {workers.map((c) => <option value={c.id}>{c.surname} {c.name} {c.email}</option>)}
              </select><br></br>
              <input type="submit" value="Zapisz"></input>
            </form>
          )
          : <p>Ładowanie...</p>
      }

    </>
  )
}

export default AssignManagerToOrder