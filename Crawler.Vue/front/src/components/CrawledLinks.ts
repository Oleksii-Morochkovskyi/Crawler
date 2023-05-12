export class CrawledLinks {/* eslint-disable */
  public id: number;
  public baseUrl: string;
  public dateTime: string; 
  constructor(id: number, baseUrl: string, dateTime: string) {
    this.id = id;
    this.baseUrl = baseUrl;
    this.dateTime = dateTime;
  }
}
