<template>
  <v-card class="mx-auto" max-width="600">
    <v-list-item two-line>
      <v-list-item-content>
        <v-list-item-title class="text-h5">{{ item.name }}</v-list-item-title>
        <v-list-item-subtitle>{{ item.organisationName }}</v-list-item-subtitle>
      </v-list-item-content>
    </v-list-item>
    <v-card-text>
      <v-row align="center">
        <v-col cols="12">
          <v-text>{{ item.description }}</v-text>
        </v-col>
      </v-row>
      <v-row align="center">
        <v-col cols="4">Cost</v-col>
        <v-col cols="8" v-if="item.cost > 0">
          <v-text>
            <template v-slot:default>
              <v-span>£</v-span>
              <v-span>{{ item.cost }}</v-span>
            </template>
          </v-text>
        </v-col>
        <v-col cols="8" v-else>
          FREE
        </v-col>
      </v-row>
      <v-row align="">
        <v-col cols="4">Distance</v-col>
        <v-col cols="8">
          <v-text>
            <template v-slot:default>
              <v-span class="pr-1">{{ item.howFarAway }}</v-span>
              <v-span >KM</v-span>
            </template>
          </v-text>
        </v-col>
      </v-row>
      <v-row align="start">
        <v-col cols="4">Location</v-col>
        <v-col cols="8">
          <v-text>{{ item.addressLine1 }}</v-text><br/>
          <v-text>{{ item.addressLine2 }}</v-text><br/>
          <v-text>{{ item.townOrCity }}</v-text><br/>
          <v-text>{{ item.county }}</v-text><br/>
          <v-text>{{ item.postcode }}</v-text>
        </v-col>
      </v-row>
    </v-card-text>
    <v-divider/>
    <v-card-actions class="justify-end">
      <v-btn text @click="gotoServiceDetails(item.id)">
        Details
      </v-btn>
    </v-card-actions>
  </v-card>
  <v-divider/>
</template>

<script setup>
defineProps(['item']);

function gotoServiceDetails(serviceId) {
  console.log(`Go to service ${serviceId} details`);
}
</script>

<style scoped>

</style>
