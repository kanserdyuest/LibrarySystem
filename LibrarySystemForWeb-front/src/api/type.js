import request from '../utils/request'

export function getTypeList(query) {
  return request({
    method: 'GET',
    url: '/Type/GetListInfo',
    params: query
  })
}

export function getAllTypeList() {
  return request({
    method: 'GET',
    url: '/Type/GetAllInfo'
  })
}

export function saveType(data) {
  return request({
    method: 'POST',
    url: '/Type/SaveInfo',
    data: data
  })
}

export function editType(data) {
  return request({
    method: 'POST',
    url: '/Type/UpdateInfo',
    data: data
  })
}

export function removeType(BtId) {
  return request({
    method: 'POST',
    url: '/Type/RemoveInfo?BtId=' + BtId
  })
}
