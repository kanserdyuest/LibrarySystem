import request from "../utils/request";

export function getBookList(query) {
  return request({
    method: 'GET',
    url: '/Book/GetList',
    params: query
  })
}

export function saveBook(data) {
  return request({
    method: 'POST',
    url: '/Book/SaveInfo',
    data: data
  })
}

export function editBook(data) {
  return request({
    method: 'POST',
    url: '/Book/UpdateInfo',
    data: data
  })
}

export function removeBook(BiId) {
  return request({
    method: 'POST',
    url: '/Book/RemoveInfo',
    data: {
      BiId: BiId
    }
  })
}


export function upInfo(BiId) {
  return request({
    method: 'POST',
    url: '/Book/UpInfo',
    data: {
      BiId: BiId
    }
  })
}