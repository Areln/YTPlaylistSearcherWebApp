export interface IVideoCommentDTO {
  id: number,
  videoID: number,
  comment: string,
  user: IUserDTO,
  createdDate: string,
  isMemberOnly: boolean,
  canDelete: boolean,
}

export interface IUserDTO {
  userID: number,
  profilePicture: string,
  userName: string,
}
