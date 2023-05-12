<!-- eslint-disable -->
<template>
  <div class="container main-div">
    <form @submit.prevent="crawlLink">
      <div class="form-group">
        <label for="url-input">Enter Url:</label>
        <input type="text" id="url-input" class="form-control" v-model.trim="inputUrl" />
        <span v-if="error" class="text-danger">{{ error }}</span>
        <button type="submit" class="btn btn-primary" :disabled="isMakingRequest">Submit</button>
      </div>
    </form>
    <h2>Crawled links</h2>

    <div v-if="links.length">
      <table  class="table">
        <thead>
          <tr>
            <th>Url</th>
            <th>Date</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="link in links" :key="link.id">
            <td>{{ link.baseUrl }}</td>
            <td>{{ formatDate(link.dateTime) }}</td>
            <td>
              <router-link :to="{ name: 'result', params: { id: link.id } }">
                View
              </router-link>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import axios from "axios";
import { CrawledLinks } from "@/components/CrawledLinks";
import router from "@/router";

export default defineComponent({
  data() {
    const links = ref<CrawledLinks[]>([]);
    const error = ref("");
    const inputUrl = ref("");
    const isMakingRequest = false;
    return {
      error,
      inputUrl,
      links: [],
      isMakingRequest,
    };
  },
  created() {
    this.crawledUrls();
  },
  methods: {
    formatDate(date: string) {
      return new Date(date).toLocaleString();
    },
    async crawlLink() {
      try {
        this.isMakingRequest = true;
        const body = {
          url: this.inputUrl,
        };
        console.log(body); // eslint-disable-next-line
        const response = await axios.post("https://localhost:7270/api/Crawler", body);
        const linkId = response.data;

        router.push({ name: "result", params: { id: linkId } });
      } catch (e) {
        this.error = "Error occurred while crawling the website.";
      } finally {
        this.isMakingRequest = false;
      }
    },
    async crawledUrls() {
      try {
        const response = await axios.get("https://localhost:7270/api/Result");
        const data = response.data;
        this.links = data.map(
          (link: any) => new CrawledLinks(link.id, link.baseUrl, link.dateTime)
        );
      } catch (e) {
        this.error = "Error occurred while crawling the website.";
      }
    },
  },
});
</script>
