import axios from 'axios'
import { Message } from 'element-ui'

const request = axios.create({
    baseURL: 'api',
    timeout: 10000,
})

request.interceptors.response.use(
    response => {
        if (response.data.code === 500) {
            Message.error('接口异常')
        }else{
            return response;
        }
    })

export default request
