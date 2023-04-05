import request from '../utils/request'

export function getBookTypeChatrs() {
    return request({
        url:'/Charts/GetBookTypeChart',
        method:'GET'
    })
}


export function getBookNumChatrs() {
    return request({
        url:'/Charts/GetBookNumChart',
        method:'GET'
    })
}

export function getBorrowTop(){
    return request({
        url:'/Charts/GetBorrowTopChart',
        method:'GET'
    })
}