import request from '../utils/request'

export function getReaderList(query) {
  return request({
    method: 'GET',
    url: '/Reader/GetListInfo',
    params: query
  })
}

export function saveReader(data) {
  return request({
    method: 'POST',
    url: '/Reader/SaveInfo',
    data: data
  })
}

export function editReader(data) {
  return request({
    method: 'POST',
    url: '/Reader/UpdateInfo',
    data: data
  })
}

export function  removeReader(RId){
  return request({
    method:'POST',
    url:'/Reader/RemoveInfo?RId='+RId
  })
}
