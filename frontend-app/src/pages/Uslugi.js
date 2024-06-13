import React from 'react'
import { useState, useEffect } from 'react'
import './Uslugi.css'
import axios from '../api/axios';
import WidokUslugi from '../components/WidokUslugi';

import pc from '../image/pc.png'
import laptop from '../image/laptop.png'
import phone from '../image/iphone.png'
import television from '../image/television.png'
import ps4 from '../image/ps4.png'
import { Navbar } from '../components/Navbar';

const SERVICE_URL = '/api/ServiceType'

function Uslugi() {

  const [options, setOptions] = useState(1);
  const [value, setValue] = useState([]);

  const getService = async () => {
    try {
      const response = await axios.get(SERVICE_URL)
      setValue(response?.data)
    } catch (err) {
      console.log(err);
    }
  }

  useEffect(() => {
    const TabBtn = document.getElementsByTagName('button')
    for (let i = 0; i < TabBtn.length; i++) {
      TabBtn[i].style.backgroundColor = 'white'
      TabBtn[i].style.color = 'rgb(1, 3, 107)'
    }

    document.getElementById(options).style.backgroundColor = 'rgb(1, 3, 107)'
    document.getElementById(options).style.color = 'white'

    getService()

  }, [options])

  return (
    <>
      <Navbar />
      <div className='main_uslugi'>
        <div className='side_bar'>
          <h2>Filtr usług</h2>
          <ul>
            <li>
              <button id={1} onClick={() => setOptions(1)}>
                Wszystkie
              </button>
            </li>
            <li>
              <button id={2} onClick={() => setOptions(2)}>
                <p className='pimg'>
                  <img src={pc} alt='' />
                </p>
                Komputer
              </button>
            </li>
            <li>
              <button id={3} onClick={() => setOptions(3)}>
                <p className='pimg'>
                  <img src={laptop} alt='' />
                </p>
                Laptop
              </button>
            </li>
            <li>
              <button id={4} onClick={() => setOptions(4)}>
                <p className='pimg'>
                  <img src={phone} alt='' />
                </p>
                Telefon
              </button>
            </li>
            <li>
              <button id={5} onClick={() => setOptions(5)}>
                <p className='pimg'>
                  <img src={television} alt='' />
                </p>
                Telewizor
              </button>
            </li>
            <li>
              <button id={6} onClick={() => setOptions(6)}>
                <p className='pimg'>
                  <img src={ps4} alt='' />
                </p>
                Konsola
              </button>
            </li>
          </ul>
        </div>
        <div className='opt_view'>
          {
            value ? value.map((el) => <WidokUslugi name={el} key={el.id} />) : <></>
          }
        </div>
      </div>
    </>
  )
}

export default Uslugi
