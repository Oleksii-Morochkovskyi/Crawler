<template>
  <div class="container main-div">
    <h1 class="header">List of URLs with Response Time</h1>
    <table class="table">
      <thead>
        <tr>
          <th>Url</th>
          <th>Response Time (ms)</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="childUrl in linkPerformance?.foundUrls" :key="childUrl.id">
          <td>{{ childUrl?.url }}</td>
          <td>{{ childUrl?.responseTimeMs }}</td>
        </tr>
      </tbody>
    </table>
    <table class="table">
      <thead>
        <tr>
          <th>Urls that not found at website</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="link in linkPerformance?.urlsFromXml" :key="link">
          {{
            link
          }}
        </tr>
      </tbody>
    </table>
    <table class="table">
      <thead>
        <tr>
          <th>Urls that not found at sitemap.xml</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="link in linkPerformance?.urlsFromHtml" :key="link">
          {{
            link
          }}
        </tr>
      </tbody>
    </table>
    <router-link :to="{ name: 'home' }">Go back</router-link>
  </div>
</template>

<script lang="ts">
import "bootstrap/dist/css/bootstrap.css";
import "bootstrap-vue/dist/bootstrap-vue.css";
import { defineComponent, ref } from "vue";
import axios from "axios";
import { LinkPerformance } from "@/components/LinkPerformance";

export default defineComponent({
  data() {
    const linkId = this.$route.params.id;
    const linkPerformance = ref<LinkPerformance>();
    const error = ref("");
    return {
      error,
      linkId,
      linkPerformance,
    };
  },
  created() {
    this.getLinkPerformance();
  },
  methods: {
    async getLinkPerformance() {
      try {
        const response = await axios.get(
          `https://localhost:7270/api/Result/${this.linkId}`
        );
        console.log(response);
        const data = response.data;
        Object.assign(this, {
          linkPerformance: data,
        });
        console.log("this.linkPerformance?.urls");
        console.log(this.linkPerformance);
      } catch (e) {
        this.error = "Error occurred while crawling the website.";
      }
    },
  },
});
</script>
