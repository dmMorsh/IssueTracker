export interface IAuthUser {

    login: string
    password: string
}

export interface IUser {

    id: string
    username: string
    token: string
    refreshToken: string
    role?: string
}