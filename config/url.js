import env from './env'

const DEV_URL = 'http://localhost:9001/api'
const PRO_URL = 'http://localhost:9001/api'

export default env === 'development' ? DEV_URL : PRO_URL
