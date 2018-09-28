import env from './env'

const DEV_URL = 'http://localhost:9001/api'
const PRO_URL = 'http://e.mdsd.cn:9000/api'
//production
export default env === 'development' ? DEV_URL : PRO_URL
