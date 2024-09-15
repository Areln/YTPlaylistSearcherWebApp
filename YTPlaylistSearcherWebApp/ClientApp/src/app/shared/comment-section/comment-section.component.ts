import { Component, Input } from '@angular/core';
import { PlaylistService } from '../../services/PlaylistService';
import { IVideoCommentDTO } from '../../DTOs/IVideoCommentDTO';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-comment-section',
  templateUrl: './comment-section.component.html',
  styleUrls: ['./comment-section.component.css']
})
export class CommentSectionComponent {

  @Input() videoID!: number;

  comments!: IVideoCommentDTO[];
  commentForm!: FormGroup;

  constructor(private service: PlaylistService, private formBuilder: FormBuilder,)
  {
    this.commentForm = this.formBuilder.group({
      commentInput: ['', Validators.required],
    });
  }

  loadComments() {
    this.service.GetVideoComments(this.videoID).subscribe(results => {
      this.comments = results;
    });
  }

  createComment() {
    if (this.commentForm.valid) {
      this.service.CreateComment(this.videoID, this.commentForm.controls.commentInput.value as string).subscribe(x => {
        this.loadComments();
        this.commentForm.controls.commentInput.patchValue(null, { emitEvent: false });
      });
    }
  }

  deleteComment(commentID: number) {
    this.service.DeleteComment(commentID).subscribe(x => { this.loadComments(); });
  }
}
