import { useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import './App.css'
import Dashboard from '@/components/layout/index'

function App() {
  // const [count, setCount] = useState(0)

  return (
    <>
    <Dashboard />
      {/* <div className='flex items-center justify-center'>
        <a href="https://vite.dev" target="_blank">
          <img src={viteLogo} className="w-32 h-auto" alt="Vite logo" />
        </a>
        <a href="https://react.dev" target="_blank">
          <img src={reactLogo} className="logo react" alt="React logo" />
        </a>
      </div>
      <h1>Vite + React</h1>
      <div className="bg-red-500 text-white px-4 py-2 rounded">
        <button onClick={() => setCount((count) => count + 1)}>
          count is {count}
        </button>
        <p>
          Edit <code>src/App.tsx</code> and save to test HMR
        </p>
      </div>
      <p className="read-the-docs">
        Click on the Vite and React logos to learn more
      </p> */}

    </>
  )
}

export default App
