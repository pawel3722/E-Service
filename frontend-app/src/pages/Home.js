import React from 'react'
import './Home.css'
import zdj1 from '../image/zdj1.jpg'
import arrow from '../image/right-arrow.png'
import serwis from '../image/serwis.jpg'
import { Link } from 'react-router-dom'

function Home() {
  return (
    <div className='main'>
        <div className='info'>
            <h1 className='h1_home'>
            Kim jesteśmy?
            </h1>
            <p className='tekst'>
            Nasz zespół składa się z ekspertów z różnych dziedzin, 
            od programistów i inżynierów po specjalistów od marketingu i treści. 
            Każdy z nas wnosi do projektu unikalne spojrzenie i umiejętności, 
            co pozwala nam na kompleksowe podejście do tworzenia i prowadzenia serwisu elektronicznego.
            </p>
        </div>
        <div className='grafika'>
            <img src={zdj1} alt="Grafika" className='home_image'/>
        </div>

        <div className='grafika'>
            <img src={serwis} alt="Grafika" className='home_image'/>
        </div>
        <div className='info'>
            <h1 className='h1_home'>
            Co robimy?
            </h1>
            <p className='tekst'>
            Naszym celem jest dostarczanie Wam najświeższych informacji, praktycznych porad, 
            recenzji produktów oraz inspirujących treści z zakresu elektroniki, technologii i nowych trendów. 
            Nieustannie pracujemy nad poszerzaniem naszej wiedzy i umiejętności, 
            aby móc dostarczać Wam treści najwyższej jakości.
            </p>
            <Link to='/uslugi' className='ulink'>
                <img src={arrow} alt=">" width={15}/>
                Oferowane usługi
            </Link>
        </div>
    </div>
  )
}

export default Home
