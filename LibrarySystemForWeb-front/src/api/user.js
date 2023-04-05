import request from '../utils/request'

export function getUserList(query) {
  return request({
    method: 'GET',
    url: '/User/GetListInfo',
    params: query
  })
}

export function saveUser(data) {
  return request({
    method: 'POST',
    url: '/User/SaveInfo',
    data: data
  })
}

export function editUser(data) {
  return request({
    method: 'POST',
    url: '/User/UpdateInfo',
    data: data
  })
}

export function removeUser(UId) {
  return request({
    method: 'POST',
    url: '/User/RemoveInfo?UId=' + UId
  })
}

export function updateUIdentity(UId, UIdentity) {
  return request({
    method: 'POST',
    url: '/User/UpdateUIdentity',
    data: {
      UId: UId,
      UIdentity: UIdentity
    }
  })
}


export function updatePwd(UId, UPwd) {
  return request({
    method: 'POST',
    url: '/User/UpdatePwd',
    data: {
      UId: UId,
      UPwd: UPwd
    }
  })
}