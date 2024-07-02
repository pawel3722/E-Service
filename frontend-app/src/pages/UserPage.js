import React from 'react'
import { useState, useEffect } from "react";
import useAxiosPrivate from "../hooks/useAxiosPrivate";
import { useNavigate, useLocation, Outlet } from "react-router-dom";
import ClientHomePageWidget from '../components/ClientHomePageWidget';
import NavbarUser from '../components/NavbarUser';

function UserPage() {
  const [users, setUsers] = useState();
  const axiosPrivate = useAxiosPrivate();
  const navigate = useNavigate();
  const location = useLocation();

  useEffect(() => {
    // let isMounted = true;
    // const controller = new AbortController();

    // const getUsers = async () => {
    //   try {
    //     const response = await axiosPrivate.get('/api/Auth/users/me', {
    //       signal: controller.signal
    //     });
    //     console.log(response.data);
    //     isMounted && setUsers(response.data);
    //   } catch (err) {
    //     console.error(err);
    //     navigate('/log', { state: { from: location }, replace: true });
    //   }
    // }

    // getUsers();

    // return () => {
    //   isMounted = false;
    //   controller.abort();
    // }

    const getUsers = async () => {
      try {
        const response = await axiosPrivate.get('/api/Auth/users/me')
        console.log(response.data)
        setUsers(response.data)
      } catch (err) {
        console.error(err)
        navigate('/log', { state: { from: location }, replace: true })
      }
    }

    getUsers()

  }, [])

  return (
    <>
      <NavbarUser />
      <div>UserPage</div>
      {
        users
          ? 
          (
            // <p>Rola: { users.roles[0].name }</p>
             users.roles[0].name == "Client"
              ?  <ClientHomePageWidget />
              : <p>Brak klienta</p>
          )
          : <p>Brak roli</p>
      }
      <Outlet />
           
      </>
  )
}

export default UserPage