import React from 'react'
import AuthContext from '../context/AuthProvider'
import { useContext } from 'react'

function AdminPage() {
    const data = useContext(AuthContext)

  return (
    <>
        <div>AdminPage</div>
        <p>{data.auth.email}</p>
        <p></p>
        <p>{JSON.stringify(data)}</p>
    </>
  )
}

export default AdminPage