import { Component, Inject } from '@angular/core';
import { HttpClient, HttpEventType } from '@angular/common/http';
//import { Http } from '@angular/http';
import { FormGroup, FormControl } from '@angular/forms';
import { Validators } from '@angular/forms';
import { error } from 'protractor';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})

export class HomeComponent {
  public dReport: DriversReport[];

  private upldFile: File = null;

  mainForm = new FormGroup({
    statusText: new FormControl(''),
    fileInfoText: new FormControl('', Validators.required),
    fileInput: new FormControl('')
  });

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    //this.baseURL = baseUrl;  
  }

  public ValidateFile() {
    //Add further validation if required
    return true;    //Do submit
  }

  public onSubmit() {
    //alert(this.upldFile.name);
    if (this.upldFile != null) {
      const formData = new FormData();
      formData.append('file', this.upldFile, this.upldFile.name);
     
      //this.http.post<ReportRow[]>(this.baseUrl + "Home", formData).subscribe(
      this.http.post<DriversReport[]>(this.baseUrl + "Home", formData).subscribe(
        result => {
          if (result != null) {
            this.mainForm.patchValue({ statusText: 'File processed OK' });
            //alert(result.length); 
            this.dReport = result;
          }
          else {
            this.mainForm.patchValue({ statusText: 'Error processing file' });
            this.dReport = null;
          }
        },
        error => {
          this.mainForm.patchValue({ statusText: 'Error: ' + error.message });
        }
      );
    }
  }


  //**** File Upload button functions ****
  //Called when the value of the file input changes
  onFileSelect(input: HTMLInputElement): void {
    if (input.files.length == 1) {
      const file = input.files[0];
      this.mainForm.controls['fileInfoText'].setValue(`${file.name} (${formatBytes(file.size)})`);
      this.upldFile = file;
    }
    else
      this.upldFile = null;

    //Calculate file size
    function formatBytes(bytes: number): string {
      const UNITS = ['Bytes', 'kB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'];
      const factor = 1024;
      let index = 0;

      while (bytes >= factor) {
        bytes /= factor;
        index++;
      }

      return `${parseFloat(bytes.toFixed(2))} ${UNITS[index]}`;
    }
  }

  onClick(input: HTMLInputElement, $event) {
    $event.preventDefault();  //prevent form submit
    input.click();            //simulate click on file input button
  }
  //**********************************
}


interface DriversReport {
  field1: string;
}

