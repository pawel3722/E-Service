import { Route, Routes } from 'react-router-dom';
import './App.css';
import { Navbar } from './components/Navbar'
import Uslugi from './pages/Uslugi';
import Kontakt from './pages/Kontakt';
import Log from './pages/Log';
import Home from './pages/Home';
import AdminPage from './pages/AdminPage';


function App() {
  return (
    <div className="App">
      <Navbar />
      <Routes>
        <Route path="/" element={<Home />}/>
        <Route path="/home" element={<Home />} />
        <Route path="/uslugi" element={<Uslugi />} />
        <Route path="/kontakt" element={<Kontakt />} />
        <Route path="/log" element={<Log />} />
        <Route path='/admin' element={<AdminPage />} />
      </Routes>
    </div>
  );
}

export default App;
