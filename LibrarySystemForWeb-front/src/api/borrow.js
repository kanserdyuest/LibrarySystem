import request from '../utils/request'

export function getBorrowList(query) {
  return request({
    method: 'GET',
    url: '/Borrow/GetBorrowList',
    params: query
  })
}

export function AddBorrow(data) {
  return request({
    method: 'POST',
    url: '/Borrow/AddBorrow',
    data: data
  })
}

export function editBorrow(data) {
  return request({
    method: 'POST',
    url: '/Borrow/UpdateInfo',
    data: data
  })
}

export function Renewborrow(data) {
  return request({
    method: 'POST',
    url: '/Borrow/Renewborrow',
    data: data
  })
}